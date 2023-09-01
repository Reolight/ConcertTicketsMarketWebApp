import ApiAuthorzationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { Home } from "./components/Home";
import PerformerNew from './components/Performers/PerformerNew';
import Performers from './components/Performers/Performers';

export const RouteParts = {
  performers: '/performers',
  update: '/update',
  concerts: '/concerts'
}

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: RouteParts.performers,
    element: <Performers />
  },
  {
    path: `${RouteParts.performers}${RouteParts.update}`,
    element: <PerformerNew />
  },
  ...ApiAuthorzationRoutes
];

export default AppRoutes;
