import React, { useEffect, useState } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import { LoginMenu } from './api-authorization/LoginMenu';
import './NavMenu.css';
import { useSelector } from 'react-redux';
import { selectUser } from '../features/reducer/userSlice';

// page links: text, role (undef, u, a), route;
const pageLinks = [
  { text: 'Concerts', route: '/'},
  { text: 'Performers', route: '/performers'},
  { text: 'User panel', role: 'admin', route: '/users'},
  { text: 'Discounts', role: 'admin', route: '/discounts'}
]

export default function NavMenu(props) {
  const [collapsed, setCollapsed] = useState(true);

  const user = useSelector(store => store.user);

  function toggleNavbar() {
    setCollapsed(!collapsed);
  }

  return (
    <header>
      <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
        <NavbarBrand tag={Link} to="/">ConcertTicketsMarket</NavbarBrand>
        <NavbarToggler onClick={toggleNavbar} className="mr-2" />
        <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar>
          <ul className="navbar-nav flex-grow">

            {pageLinks
              .filter(link => !link.role || (!!user && link.role === user.role))
              .map((link, index) => <NavItem key={10 + index}>
                <NavLink key={index} tag={Link} className="text-dark" to={link.route}>{link.text}</NavLink>
              </NavItem>
            )}

            <LoginMenu />
          </ul>
        </Collapse>
      </Navbar>
    </header>
  );
}
