import React, { Component } from 'react';
import { Link } from 'react-router-dom';

export class Users extends Component {
  displayName = Users.name

  constructor(props) {
    super(props);
    this.state = { forecasts: [], loading: true };

    fetch('http://localhost:5000/users')
      .then(response => response.json())
      .then(data => {
        this.setState({ allUsers: data, loading: false });
      });
  }

  renderUsersTable(users) {
    return (
      <table className='table'>
        <thead>
          <tr>
            <th>Username</th>
          </tr>
        </thead>
        <tbody>
          <tr>
            <td>
                <Link to={'/createuser'}>Add</Link>
            </td>
          </tr>
          {users.map(user =>
            <tr key={user.id}>
              <td>{user.username}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : this.renderUsersTable(this.state.allUsers);

    return (
      <div>
        <h1>Users</h1>
        {contents}
      </div>
    );
  }
}
