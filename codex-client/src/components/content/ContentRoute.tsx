import { observer } from "mobx-react-lite";
import { useEffect, useRef, useState } from "react";
import { useParams } from "react-router-dom";
import { Grid, Header, Loader, Ref, Sticky } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import SectionNavigator from "./reader/section/SectionNavigator";
import SectionReader from "./reader/section/SectionReader";
import AbstractTermDetails from "./termDetails/AbstractTermDetails";
import '../styles/styles.css';

export default observer(function ContentRoute() {
    const {contentId, index} = useParams();
    const {contentStore} = useStore();
    const {loadSectionById, currentSectionTerms, sectionLoaded} = contentStore;
    const objectRef = useRef<HTMLElement>(document.activeElement as HTMLElement);
    const [functionalRef, setFunctionalRef] = useState<HTMLElement | null>(null);
    const [isMounted, setIsMounted] = useState(false)

  useEffect(() => {
    loadSectionById(contentId!, parseInt(index!));
    setIsMounted(true);
    //objectRef.current?.focus();
    return () => setIsMounted(false)
  }, [loadSectionById, contentId, index]);

    return (
        <div>
            <Grid>
                <Grid.Column width={10}>
                    <Header content={` node value: ${objectRef.current?.nodeValue} node type: ${objectRef.current?.nodeType} node name: ${objectRef.current?.nodeName}` } />
                    {
                        sectionLoaded? 
                        (
                            <div>
                                <SectionNavigator contentId={contentId!} currentIndex={parseInt(index!)} />
                                <SectionReader section={currentSectionTerms} />
                            </div>
                            
                         ) : (
                            <Loader />
                         )
                    }
                    
                </Grid.Column>
                <Grid.Column width={6} >
                    <Ref innerRef={setFunctionalRef}>
                        <Sticky offset={40} context={functionalRef}>
                            <AbstractTermDetails />
                        </Sticky>
                    </Ref>
                    
                </Grid.Column>
            </Grid>
            

        </div>
    )
})