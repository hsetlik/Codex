import React from "react";
import { Link } from "react-router-dom";
import { Container, Header } from "semantic-ui-react";

export default function HomeRoute(){
    return (
        <Container>
                <Header as={Link} to="../account/login" content="Login" className="label" />
                <Header as={Link} to="../account/register" content="Register" className="label" />
        </Container>
    )
}