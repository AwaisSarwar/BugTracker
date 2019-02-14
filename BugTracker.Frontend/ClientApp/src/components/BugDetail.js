import React, { Component } from 'react';
import { Col, Grid, Row } from 'react-bootstrap';

export class BugDetail extends Component {
  displayName = BugDetail.name

  constructor(props) {
    super(props);
    this.state = { users: [], bug: {}, loading: true };
    this.handleSubmit = this.handleSubmit.bind(this);
    this.closeBug = this.closeBug.bind(this);

    this.handleTitleChange = this.handleTitleChange.bind(this);
    this.handleDescriptionChange = this.handleDescriptionChange.bind(this);
    this.handleSeverityChange = this.handleSeverityChange.bind(this);
    this.handleAssignedToChange = this.handleAssignedToChange.bind(this);
    this.renderBugForm = this.renderBugForm.bind(this);


    fetch('http://localhost:5000/users')
      .then(response => response.json())
      .then(data => {
        this.setState({ users: data });
      });

    fetch('http://localhost:5000/bugs/' + props.location.state.bugId)
      .then(response => response.json())
      .then(data => {
        this.setState({ bug: data, loading: false });
      });
  }

  closeBug(event){
    event.preventDefault();
    let bug = this.state.bug;

    fetch('http://localhost:5000/bugs/' + bug.id + '/close',{
        method: "PATCH",
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
      }).then(response => {
         if(response.status === 200){
          alert("Bug closed successfully");
         }
    });
  }

  handleTitleChange(event){
    let bug = this.state.bug;
    bug.title = event.target.value
    this.setState({ bug: bug });
  }

  handleDescriptionChange(event){
    let bug = this.state.bug;
    bug.description = event.target.value
    this.setState({ bug: bug });
  }

  handleSeverityChange(event){
    let bug = this.state.bug;
    bug.severity = event.target.value
    this.setState({ bug: bug });
  }

  handleAssignedToChange(event){
    let bug = this.state.bug;
    bug.assignedTo = event.target.value
    this.setState({ bug: bug });
  }

  handleSubmit(e) {
    e.preventDefault();
    let bug = this.state.bug;

    fetch('http://localhost:5000/bugs',{
        method: "PUT",
        body: JSON.stringify(bug),
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
      }).then(response => {
        if(response.status === 200){
          alert("Bug updated successfully");
        }
    });
  }

  renderBugForm(bug) {
    return (
        <form onSubmit={this.handleSubmit}>

            <Grid>
                <Row style={{ paddingBottom: 10 }}>
                    <Col md={4}>
                        <label>
                          Title:
                        </label>
                    </Col>
                    <Col md={8}>
                        <input type="text" name="title" value={this.state.bug.title} onChange={this.handleTitleChange} style={{width: 300}}/>
                    </Col>
                </Row>
                <Row style={{ paddingBottom: 10 }}>
                    <Col md={4}>
                        <label>
                          Description:
                        </label>
                    </Col>
                    <Col md={8}>        
                        <textarea value={this.state.bug.description} onChange={this.handleDescriptionChange} style={{height: 300, width: 300}}/>
                    </Col>
                </Row>
                <Row style={{ paddingBottom: 10 }}>
                    <Col md={4}>
                        <label>
                          Severity:
                        </label>
                    </Col>
                    <Col md={8}>        
                        <select name="severity" value = {this.state.bug.severity} onChange={this.handleSeverityChange} >
                            <option value="0">Low</option>
                            <option value="1">Medium</option>
                            <option value="2">High</option>
                        </select>
                    </Col>
                </Row>
                <Row style={{ paddingBottom: 10 }}>
                    <Col md={4}>
                        <label>
                          Assigned to:
                        </label>
                    </Col>
                    <Col md={8}>        
                        <select name="assignedTo" value={this.state.bug.assignedTo} onChange={this.handleAssignedToChange} >
                            <option value="">Unassigned</option>
                            {this.state.users.map(user =>
                                <option value={user.username}>{user.username}</option>
                            )}
                        </select>
                    </Col>
                </Row>
                <Row style={{ paddingBottom: 10 }}>
                    <Col md={4}>
                    </Col>
                    <Col md={1}>
                        <input type="submit" value="Update" />
                    </Col>
                    <Col md={7}>        
                        <button type="button" onClick={this.closeBug}>Close bug</button>
                    </Col>
                </Row>
            </Grid>
        </form>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : this.renderBugForm(this.state.bug);

    return (
      <div>
        <h1>Bug Detail</h1>
        {contents}
      </div>
    );
  }
}
