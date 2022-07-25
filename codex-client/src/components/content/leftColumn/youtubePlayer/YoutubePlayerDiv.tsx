import { observer } from "mobx-react-lite";
import { createRef, useState } from "react";
import ReactPlayer from "react-player/youtube";
import { VideoCaptionElement } from "../../../../app/models/content";
import { useStore } from "../../../../app/stores/store";
import CaptionDiv from "./CaptionDiv";

export default observer(function YoutubePlayerDiv() {
    const {videoStore, termStore} = useStore();
    const {loadForMs} = videoStore;
    const handleSeek = async (seconds: number) => {
        const ms = (seconds * 1000);
        await loadForMs(ms);
    }
    const playerRef = createRef<ReactPlayer>();
    const handleProgress = (state: {
        played: number;
        playedSeconds: number;
        loaded: number;
        loadedSeconds: number; 
    }) => {
        handleSeek(state.playedSeconds);
    }
    const [isPlaying, setIsPlaying] = useState(false);
    const handleCaptionJump = (caption: VideoCaptionElement) =>
    {
        playerRef.current?.seekTo((caption.startMs || 0 / 1000), "seconds");
        setIsPlaying(true);
    }
    const handlePlay = () => setIsPlaying(true);
    const handlePause = () => setIsPlaying(false);
    return (
        <div>
           <ReactPlayer 
           url={termStore.selectedContent.contentUrl} 
           controls={true}
           onSeek={handleSeek}
           onProgress={handleProgress}
           onPlay={handlePlay}
           onPause={handlePause}
           progressInterval={300}
           ref={playerRef}
           playing={isPlaying}
           light
           />
        <CaptionDiv handleJump={handleCaptionJump} />
        </div>
    )
})