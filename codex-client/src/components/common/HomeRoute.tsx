import React from "react";
import "../styles/home.css";

export default function HomeRoute(){
    return (
        <div className="container">
            <div className="conatiner row">
                <h1 className="display-1">Welcome to Codex</h1>
                <h3>Language learning for the real world</h3>
            </div>
            <div className="container row">
                <a className="btn btn-primary landing-button" href="/account/login" >Login</a>
            </div>
             <div className="container row">
                <a className="btn btn-primary landing-button" href="/account/register">Register</a>
            </div>
    
        </div>
    )
}