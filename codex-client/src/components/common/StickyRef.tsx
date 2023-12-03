import React, { useState, useEffect, useRef } from 'react';
import '../styles/styles.css';

interface StickyProps {
    child: React.ReactNode,
    top: number,
}

const  StickyRef: React.FunctionComponent<StickyProps> = (child, top) => {
  const [isSticky, setSticky] = useState(true);
  const ref = useRef<HTMLDivElement>(null);
  
  useEffect(() => {
    const handleScroll = () => {
        // Make sure ref has current attibute and getBoundingClientRect function
        // otherwise, it could cause getBoundingClientRect undefined error.
        if (ref && ref.current && ref.current.getBoundingClientRect()) {
            // Then, we compare the distance of the ref component to the top
            // with top value we set. If less than, we set isStick ture.
            setSticky(ref.current.getBoundingClientRect().top <= top);
        }
      };
    // Add scroll event when the component is loaded
    window.addEventListener('scroll', handleScroll);
    return () => {
    // Remove scroll event after the component is unmount,
    // like componentWillUnmount()
      window.removeEventListener('scroll', () => handleScroll);
    };
    // Here, we set useEffect() arguments as empty array.
    // This means useEffect() will only run once after component loaded
  }, [ref, top]);

  

  return (
      <div className={`sticky__wrapper ${isSticky && 'sticky'}`} ref={ref}>
        <div className="sticky--inner">
            {child.child}
        </div>
      </div>
  );
};

export default StickyRef;