import ApiAuthorzationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { Home } from "./components/Home";
import PerformerNew from './components/Performers/PerformerNew';
import Performers from './components/Performers/Performers';

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: 'performers/',
    element: <Performers />
  },
  {
    path: 'performers/update',
    element: <PerformerNew />
  },
  ...ApiAuthorzationRoutes
];

export default AppRoutes;
