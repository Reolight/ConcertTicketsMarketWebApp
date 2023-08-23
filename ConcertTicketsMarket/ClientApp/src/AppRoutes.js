import ApiAuthorzationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { Home } from "./components/Home";
import PerformerNew from './components/Performers/PerformerNew';

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: 'performers/update',
    element: <PerformerNew />
  },
  ...ApiAuthorzationRoutes
];

export default AppRoutes;
