import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { CreateBug } from './components/CreateBug';
import { Users } from './components/Users';
import { BugDetail } from './components/BugDetail';
import { CreateUser } from './components/CreateUser';

export default class App extends Component {
  displayName = App.name

  render() {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/createbug' component={CreateBug} />
        <Route path='/users' component={Users} />
        <Route path='/bugdetail' component={BugDetail} />
        <Route path='/createuser' component={CreateUser} />
      </Layout>
    );
  }
}
