import React from "react";
import Particles from "react-tsparticles";
import "../../styles/home.css";

export default function AboutBackground() {
    return (
      <div className="particle-container" >
        <Particles id="tsparticles"
        options={{
          particles: {
            number: {
              value: 85,
              limit: 100
            },
            shape: {
              type: "circle"
            },
            size: {
              value: 15,
              random: true,
              anim: {
                enable: true,
                speed: 10,
                size_min: 10,
                sync: false
              }
            },
            move: {
              enable: true,
              speed: 0.8,
              direction: "none",
              random: false,
              straight: false,
              out_mode: "out",
              bounce: false
            },
            lineLinked: {
              enable: true,
              size: 3,
              distance: 150,
              frequency: 0.8
            }
          },
          retina_detect: true,
          fps_limit: 30
        }}
        />
      </div>
    )
}
