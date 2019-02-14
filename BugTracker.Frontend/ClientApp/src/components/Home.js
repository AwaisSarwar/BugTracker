import React, { Component } from 'react';
import { Link } from 'react-router-dom';

export class Home extends Component {
  displayName = Home.name

  constructor(props) {
    super(props);
    this.state = { bugs: [], loading: true };

    fetch('http://localhost:5000/bugs')
      .then(response => response.json())
      .then(bugdata => {
        this.setState({ bugs: bugdata, loading: false });
      });
  }

   renderBugsTable(bugs) {
    return (
      <table className='table'>
        <thead>
          <tr>
            <th>Title</th>
            <th>Description</th>
            <th>Severity</th>
            <th>Status</th>
            <th>Reported on</th>
            <th>Assigned to</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {bugs.map(bug =>
            <tr key={bug.id}>
              <td>{bug.title}</td>
              <td>{bug.description.substring(0, 20)}...</td>
              <td>{bug.severity === 0 ? "Low" : bug.severity === 1 ? "Medium" : "High"}</td>
              <td>{bug.status === 0 ? "Opened" : bug.status === 1 ? "Assigned" : "Closed"}</td>
              <td>{new Date(bug.reportedOn).toLocaleString()}</td>
              <td>{bug.assignedTo}</td>
              <td><Link to={{ pathname: '/bugdetail', state: { bugId: bug.id} }}>Detail</Link></td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : this.renderBugsTable(this.state.bugs);

    return (
      <div>
        <h1>Open bugs</h1>
        {contents}
      </div>
    );
  }
}
