import React, { Component, createContext } from 'react';
import { Container } from 'reactstrap';
import NavMenu from './NavMenu';

export function Layout(props) {
  const displayName = Layout.name;

  return (  
    <div>
      <NavMenu />
      <Container tag="main">
        {props.children}
      </Container>
    </div>
  );

}
