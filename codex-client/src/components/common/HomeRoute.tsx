import React from "react";
import { Link } from "react-router-dom";
import { Container, Header, Segment } from "semantic-ui-react";

export default function HomeRoute(){
    //just a placeholder to pass something in in App.tsx
    return (
        <Container>
                <Header as={Link} to="../account/login" content="Login" className="label" />
                <Header as={Link} to="../account/register" content="Register" className="label" />
        </Container>
    )
}