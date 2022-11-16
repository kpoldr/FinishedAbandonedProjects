import React from "react";
export interface IPrivacyProps {
    counter: number;
    updateCounter: () => void;
} 
function Privacy(props:IPrivacyProps) {
    return (
        <>
        <div>Counter: {props.counter}</div>
        <a className="btn btn-primnary border" onClick={() => { 
        props.updateCounter();
        console.log(props.counter);
    } }> Increment Counter</a></>
    );
    
}

export default Privacy;
