import React, { Component } from 'react';
import { Col, Grid, Row } from 'react-bootstrap';

export class CreateUser extends Component {
  displayName = CreateUser.name

  constructor(props) {
    super(props);
    this.state = { username: "", loading: false };
    this.handleSubmit = this.handleSubmit.bind(this);
    this.renderCreateUserForm = this.renderCreateUserForm.bind(this);   
    this.handleUsernameChange = this.handleUsernameChange.bind(this);
  }

  handleUsernameChange(event) {
    this.setState({ username: event.target.value });
  }

  handleSubmit(e) {
    e.preventDefault();
    let user = {Username: this.state.username};

    fetch('http://localhost:5000/users',{
        method: "POST",
        body: JSON.stringify(user),
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
      }).then(response => {
          if(response.status === 201){
            alert("User created successfully");
            this.setState({username: ""});
          }
    });
  }

  renderCreateUserForm() {
      return (
        <form onSubmit={this.handleSubmit}>
            <Grid>
                <Row>
                    <Col md={1}>
                        <label>
                          Username:
                        </label>
                    </Col>
                    <Col md={2}>
                        <input type="text" name="username" value={this.state.username} onChange={this.handleUsernameChange} />
                    </Col>
                    <Col md={2}>
                        <input type="submit" value="Create User" />
                    </Col>
                </Row>
            </Grid>
        </form>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : this.renderCreateUserForm();

    return (
      <div>
        <h1>New user</h1>
        {contents}
      </div>
    );
  }
}
