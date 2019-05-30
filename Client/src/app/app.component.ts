import { Component, OnInit } from '@angular/core';

import { environment as env } from '@env/environment';
import { Title } from '@angular/platform-browser';
import { Router, ActivationEnd, NavigationStart, NavigationEnd, NavigationCancel, NavigationError, Event } from '@angular/router';
import { filter } from 'rxjs/operators';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  envName = env.envName;
  appName = env.appName;

  loading = false;

  constructor(
    private router: Router,
    private titleService: Title,
  ) {}

  ngOnInit(): void {
    this.router.events
      .subscribe((event: Event) => {

        switch (true) {
          case event instanceof NavigationStart: {
            this.loading = true;
            break;
          }

          case event instanceof NavigationEnd:
          case event instanceof NavigationCancel:
          case event instanceof NavigationError: {
            this.loading = false;
            break;
          }
          case event instanceof ActivationEnd: {

            const eventActivationEnd = event as ActivationEnd;
            let lastChild = eventActivationEnd.snapshot;
            while (lastChild.children.length) {
              lastChild = lastChild.children[0];
            }
            const { title } = lastChild.data;
            this.titleService.setTitle(
              title ? `${title} - ${env.appName}` : env.appName
            );
            break;

          }
          default: {
            break;
          }
        }
      });
  }
}
