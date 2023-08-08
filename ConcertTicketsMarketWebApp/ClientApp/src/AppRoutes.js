import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import ApiAuthorizationRoutes from "./api-authorization/ApiAuthorizationRoutes";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/counter',
    element: <Counter />
  },
  {
    path: '/fetch-data',
    element: <FetchData />
    },
    ...ApiAuthorizationRoutes
];

export default AppRoutes;
