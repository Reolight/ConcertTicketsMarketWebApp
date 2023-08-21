import { Home } from "./components/Home";
import ApiAuthorizationRoutes from './api-authorization/ApiAuthorizationRoutes'

export const BackRoutes = {
  Concerts: '/Concerts',
  Performers: '/Performers',
  Tickets: '/Tickets'
}

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  ...ApiAuthorizationRoutes
];

export default AppRoutes;
