import React from 'react';
import { Container } from 'reactstrap';
import NavMenu from './NavMenu';
import { Provider } from 'react-redux';
import store from '../features/reducer/store';

export function Layout(props) {
  const displayName = Layout.name;

  // the first getUser triggers updateState in authService. If user exists, its data appears in reducer;
  return (  
    <div>
      <Provider store={store}>
        <NavMenu />
        <Container tag="main">
          {props.children}
        </Container>
      </Provider>
    </div>
  );

}
