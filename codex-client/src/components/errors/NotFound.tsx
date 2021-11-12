import React from "react";
import { Header, Icon, Segment, Button } from "semantic-ui-react";
import { Link } from 'react-router-dom'

export default function NotFound() {
    return(
        <Segment>
            <Header icon>
                <Icon name='search' />
                404 Not Found!!!
            </Header>
            <Segment inline>
                <Button as={Link} to='/activities' primary content='Take me Back'/>
            </Segment>
        </Segment>
    )
}