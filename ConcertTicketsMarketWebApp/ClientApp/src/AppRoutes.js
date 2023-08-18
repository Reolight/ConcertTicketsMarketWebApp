import { Home } from "./components/Home";
import ApiAuthorizationRoutes from "./api-authorization/ApiAuthorizationRoutes";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
    ...ApiAuthorizationRoutes
];

export default AppRoutes;
