import { observer } from "mobx-react-lite";
import { createRef, useState } from "react";
import ReactPlayer from "react-player/youtube";
import { ElementAbstractTerms, VideoCaptionElement } from "../../../../app/models/content";
import { useStore } from "../../../../app/stores/store";

export default observer(function YoutubePlayerDiv() {
    const {videoStore, contentStore: {selectedContentUrl}} = useStore();
    const {loadForMs} = videoStore;
    const handleSeek = (seconds: number) => {
        const ms = (seconds * 1000);
        loadForMs(ms);
    }
    const playerRef = createRef<ReactPlayer>();
    const handleProgress = (state: {
        played: number;
        playedSeconds: number;
        loaded: number;
        loadedSeconds: number; 
    }) => {
        const playedMs = state.playedSeconds * 1000;
        loadForMs(playedMs);
    }
    const [isPlaying, setIsPlaying] = useState(false);
    const handleCaptionJump = (caption: VideoCaptionElement) =>
    {
        playerRef.current?.seekTo((caption.startMs || 0 / 1000), "seconds");
        if(!isPlaying) {
            setIsPlaying(true);
        }
    }

    const handlePlay = () => setIsPlaying(true);
    const handlePause = () => setIsPlaying(false);
    return (
        <div>
           <ReactPlayer 
           url={selectedContentUrl} 
           controls={true}
           onSeek={handleSeek}
           onProgress={handleProgress}
           onPlay={handlePlay}
           onPause={handlePause}
           progressInterval={400}
           ref={playerRef}
           playing={isPlaying}
           light
           />
           <div>

           </div>
        </div>
    )
})