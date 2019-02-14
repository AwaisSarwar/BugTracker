import React, { Component } from 'react';
import { Col, Grid, Row } from 'react-bootstrap';

export class CreateBug extends Component {
  displayName = CreateBug.name

  constructor(props) {
    super(props);
    this.state = {Title: "", Description: "", Severity: 0, loading: false };
    this.handleSubmit = this.handleSubmit.bind(this);
    this.handleTitleChange = this.handleTitleChange.bind(this);
    this.handleDescriptionChange = this.handleDescriptionChange.bind(this);
    this.handleSeverityChange = this.handleSeverityChange.bind(this);
  }

  handleTitleChange(event) {
    this.setState({ Title: event.target.value });
  }

  handleDescriptionChange(event) {
    this.setState({ Description: event.target.value });
  }

  handleSeverityChange(event) {
    this.setState({ Severity: event.target.value });
  }

  handleSubmit(e) {
    e.preventDefault();

    let bug = { 
        Title: this.state.Title, 
        Description: this.state.Description, 
        Severity: this.state.Severity};

    fetch('http://localhost:5000/bugs',{
        method: "POST",
        body: JSON.stringify(bug),
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        }}).then(response => {
            if(response.status === 201) {
                alert("Bug created successfully");
                this.setState({Title: "", Description: "", Severity: 0});
            }
    });
  }
  
  renderCreateBugForm() {
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
                        <input type="text" name="title" value={this.state.Title} onChange={this.handleTitleChange} style={{width: 300}}/>
                    </Col>
                </Row>
                <Row style={{ paddingBottom: 10 }}>
                    <Col md={4}>
                        <label>
                          Description:
                        </label>
                    </Col>
                    <Col md={8}>
                        <textarea value={this.state.Description} onChange={this.handleDescriptionChange} style={{height: 300, width: 300}}/>
                    </Col>
                </Row>

                <Row style={{ paddingBottom: 10 }}>
                    <Col md={4}>
                        <label>
                            Severity:
                        </label>
                    </Col>
                    <Col md={4}>
                        <select name="severity" value = {this.state.Severity} onChange={this.handleSeverityChange} >
                            <option value="0">Low</option>
                            <option value="1">Medium</option>
                            <option value="2">High</option>
                        </select>
                    </Col>
                </Row>
                <Row style={{ paddingBottom: 10 }}>
                    <Col md={4}>
                    </Col>
                    <Col md={8}>
                        <input type="submit" value="Create" />
                    </Col>
                </Row>
            </Grid>
        </form>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : this.renderCreateBugForm();

    return (
      <div>
        <h1>New bug</h1>
        {contents}
      </div>
    );
  }
}
