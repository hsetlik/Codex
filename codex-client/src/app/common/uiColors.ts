import React from "react";


export interface RgbColor {
    r: number,
    g: number,
    b: number
}

export const cssString = (color: RgbColor): string => {
    return `rgb(${color.r}, ${color.g}, ${color.b} )`;
}

interface Pallette {
    primary: RgbColor,
    primaryDark: RgbColor,
    primaryLight: RgbColor,

    secondary: RgbColor,
    secondaryDark: RgbColor,
    secondaryLight: RgbColor,

    background: RgbColor,
    surface: RgbColor,
    error: RgbColor,

    lightText: RgbColor,
    darkText: RgbColor,
    
    knownColor: RgbColor,
    unknownColor: RgbColor
}

export const CodexPallette: Pallette = {
    primary: {r: 25, g: 35, b: 125},
    primaryDark: {r: 1, g: 0, b: 80},
    primaryLight: {r: 83, g: 74, b: 173},

    secondary: {r: 255, g: 182, b: 77},
    secondaryDark: {r: 200, g: 135, b: 26},
    secondaryLight: {r: 254, g: 233, b: 124},

    background: {r: 224, g: 226, b: 224},
    surface: {r: 244, g: 244, b: 246},
    error: {r: 0, g: 0, b: 0},

    lightText: {r: 255, g: 255, b: 255},
    darkText: {r: 0, g: 0, b: 0},

    knownColor: {r: 119, g: 233, b: 136},
    unknownColor: {r: 10, g: 170, b: 211},
}

export const LerpColor = (a: RgbColor, b: RgbColor, t: number): RgbColor => {
    const oR = a.r + ((b.r - a.r) * t);
    const oG = a.g + ((b.g - a.g) * t);
    const oB = a.b + ((b.b - a.b) * t);
    return {r: oR, b: oB, g: oG};
}

const Primary: React.CSSProperties = {
    'backgroundColor': cssString(CodexPallette.primary),
    'color': cssString(CodexPallette.lightText)
}

const PrimaryDark: React.CSSProperties = {
    'backgroundColor': cssString(CodexPallette.primaryDark),
    'color': cssString(CodexPallette.lightText)
}

const PrimaryLight: React.CSSProperties = {
    'backgroundColor': cssString(CodexPallette.primaryLight),
    'color': cssString(CodexPallette.lightText)
}

const Secondary: React.CSSProperties = {
    'backgroundColor': cssString(CodexPallette.secondary),
    'color': cssString(CodexPallette.darkText)
}

const SecondaryLight: React.CSSProperties = {
    'backgroundColor': cssString(CodexPallette.secondaryLight),
    'color': cssString(CodexPallette.darkText)
}

const SecondaryDark: React.CSSProperties = {
    'backgroundColor': cssString(CodexPallette.secondaryDark),
    'color': cssString(CodexPallette.darkText)
}

const Background: React.CSSProperties = {
    'backgroundColor': cssString(CodexPallette.background),
    'color': cssString(CodexPallette.darkText)
}

const Surface: React.CSSProperties = {
    'backgroundColor': cssString(CodexPallette.surface),
    'color': cssString(CodexPallette.darkText)
}



export const CssPallette = {
    Primary,
    PrimaryLight,
    PrimaryDark,
    Secondary,
    SecondaryLight,
    SecondaryDark,
    Background,
    Surface
}














