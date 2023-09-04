import ApiAuthorzationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import ConcertNew from './components/Concerts/ConcertNew';
import { Home } from "./components/Home";
import PerformerNew from './components/Performers/PerformerNew';
import Performers from './components/Performers/Performers';

export const RouteParts = {
  performers: '/performers',
  concerts: '/concerts',

  update: '/update',
  new: '/new'
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
    path: `${RouteParts.performers}${RouteParts.new}`,
    element: <PerformerNew />
  },
  {
    path: `${RouteParts.concerts}${RouteParts.new}`,
    element: <ConcertNew />
  },
  {
    path: `${RouteParts.performers}/:id${RouteParts.update}`,
    element: <PerformerNew />
  },
  {
    path: `${RouteParts.concerts}/:id${RouteParts.update}`,
    element: <ConcertNew />
  },
  ...ApiAuthorzationRoutes
];

export default AppRoutes;
