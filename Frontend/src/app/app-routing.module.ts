import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginPageComponent } from './login-page/login-page.component';
import { QueryPageComponent } from './query-page/query-page.component';
import { FunctionViewScriptComponent } from './function-view-script/function-view-script.component';
import { LayoutComponent } from './layout/layout.component';

const routes: Routes = [
  { path: '', component: LoginPageComponent },
  {
    path: '',
    component: LayoutComponent,
    children: [
      { path: 'QueryExecuter', component: QueryPageComponent },
      { path: 'ScriptRead', component: FunctionViewScriptComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule {

}
