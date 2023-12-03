import { createBrowserRouter, RouteObject } from "react-router-dom";
import FeedRoute from '../../components/feed/FeedRoute';
import LoginForm from '../../components/account/LoginForm';
import HomeRoute from '../../components/common/HomeRoute';
import RegisterForm from '../../components/account/RegisterForm';
import ProfilesRoute from '../../components/profile/ProfilesRoute';
import CollectionsRoute from '../../components/collection/CollectionsRoute';
import TagRoute from '../../components/feed/TagRoute';
import ArticleRoute from '../../components/contentFrame/ArticleRoute';
import VideoRoute from '../../components/content/leftColumn/youtubePlayer/VideoRoute';
import FeedRedirectRoute from '../../components/feed/FeedRedirectRoute';
import App from "../../App";

export const routes: RouteObject[] = [
    {
        path: '/',
        element: <App />,
        children: [
            {path: '/feed/:lang', element: <FeedRoute />},
            {path: '/feed/', element: <FeedRedirectRoute />},
            {path: '/', element: <HomeRoute />},
            {path: '/profiles/:username/:lang', element: <ProfilesRoute />},
            {path: '/account/login', element: <LoginForm />},
            {path: '/account/register', element: <RegisterForm />},
            {path: '/collections/:lang', element: <CollectionsRoute />},
            {path: '/tags/:tag', element: <TagRoute />},
            {path: '/viewer/:contentId', element: <ArticleRoute />},
            {path: '/video/:contentId', element: <VideoRoute />}
        ]
    }
]

export const router = createBrowserRouter(routes);




