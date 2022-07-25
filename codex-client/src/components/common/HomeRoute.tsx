import { observer } from "mobx-react-lite";
import React from "react";
import { useNavigate } from "react-router-dom";
import { useStore } from "../../app/stores/store";
import "../styles/home.css";
import ParticleBackground from "./landing-background/ParticleBackground";

export default observer(function HomeRoute(){
    const navigate = useNavigate();
    const {userStore} = useStore();
    if (userStore.isLoggedIn) {
        navigate(`/feed/${userStore.selectedProfile?.language}`);
    }
    return (
        <div>
            <ParticleBackground />
            <div className="container particle-foreground">
                <div className="conatiner row">
                    <h1 className="display-1 landing-header">Welcome to Codex</h1>
                    <h3 className="landing-header">Language learning for the real world</h3>
                </div>
                <div className="container row">
                    <a className="btn btn-primary landing-button" href="/account/login" >Login</a>
                </div>
                <div className="container row">
                    <a className="btn btn-primary landing-button" href="/account/register">Register</a>
                </div> 
            </div>
        </div>
        
    )
})