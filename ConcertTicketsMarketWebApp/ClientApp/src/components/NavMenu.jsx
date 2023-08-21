import * as React from 'react';
import { useEffect } from 'react';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import IconButton from '@mui/material/IconButton';
import Typography from '@mui/material/Typography';
import Menu from '@mui/material/Menu';
import MenuIcon from '@mui/icons-material/Menu';
import Container from '@mui/material/Container';
import Button from '@mui/material/Button';
import Tooltip from '@mui/material/Tooltip';
import MenuItem from '@mui/material/MenuItem';
import AdbIcon from '@mui/icons-material/Adb';
import { useSelector, useDispatch } from 'react-redux';
import { ApplicationPaths } from '../api-authorization/ApiAuthorizationConstants';
import { tryLogin } from '../features/userSlice';

//interface HandlingButton {
//  text: string,
//  role?: 'admin',
//  clue?: string,
//  onClick?: Function
//}

const pages = [
  { text: 'Concerts' },
  { text: 'Performers'},
  { text: 'Manage', role: 'admin' },
  { text: 'User Panel', role: 'admin' }
]
const settings = [
  { text: 'Logout', onClick: () => { 
    //const redir = `${window.location.origin}${ApplicationPaths.LogOut}`;
    //window.location.replace(redir);
  } 
}];

export default function NavMenu() {
  const [anchorElNav, setAnchorElNav] = React.useState(null);
  const [anchorElUser, setAnchorElUser] = React.useState(null);
  
  const user = useSelector((store) => store.user.user)

  const dispatch = useDispatch();

  useEffect(() => {
    dispatch(tryLogin())
  });

  const handleOpenNavMenu = event => {
    setAnchorElNav(event.currentTarget);
  };
  const handleOpenUserMenu = event => {
    setAnchorElUser(event.currentTarget);
  };

  const handleCloseNavMenu = () => {
    setAnchorElNav(null);
  };

  const handleCloseUserMenu = () => {
    setAnchorElUser(null);
  };

  const handleLoginClicked = () => {
    const redirectUrl = `${window.location.origin}${ApplicationPaths.Login}`;
    window.location.replace(redirectUrl);
  }

  const handleRegisterClicked = () => {
    const redirectUrl = `${window.location.origin}${ApplicationPaths.Register}`;
    window.location.replace(redirectUrl);
  }

  return (
    <AppBar position="fixed" sx={{ width: '100%' }}>
      <Container maxWidth="100vw">
        <Toolbar disableGutters>
          <AdbIcon sx={{ display: { xs: 'none', md: 'flex' }, mr: 1 }} />
          <Typography
            variant="h6"
            noWrap
            component="a"
            href="/"
            sx={{
              mr: 2,
              display: { xs: 'none', md: 'flex' },
              fontFamily: 'monospace',
              fontWeight: 700,
              letterSpacing: '.3rem',
              color: 'inherit',
              textDecoration: 'none',
            }}
          >
            CTM
          </Typography>

          <Box sx={{ flexGrow: 1, display: { xs: 'flex', md: 'none' } }}>
            <IconButton
              size="large"
              aria-label="account of current user"
              aria-controls="menu-appbar"
              aria-haspopup="true"
              onClick={handleOpenNavMenu}
              color="inherit"
            >
              <MenuIcon />
            </IconButton>
            <Menu
              id="menu-appbar"
              anchorEl={anchorElNav}
              anchorOrigin={{
                vertical: 'bottom',
                horizontal: 'left',
              }}
              keepMounted
              transformOrigin={{
                vertical: 'top',
                horizontal: 'left',
              }}
              open={Boolean(anchorElNav)}
              onClose={handleCloseNavMenu}
              sx={{
                display: { xs: 'block', md: 'none' },
              }}
            >
              {pages
              .filter(page => (page.role && page.role === user?.role || !page.role))
              .map((page) => (
                <MenuItem key={page.text} onClick={
                  () => { handleCloseNavMenu(); if (page.onClick) page.onClick(); }}>
                  <Typography textAlign="center" color={page.role? 'lightsalmon' : 'black'}>{page.text}</Typography>
                </MenuItem>
              ))}
            </Menu>
          </Box>
          <AdbIcon sx={{ display: { xs: 'flex', md: 'none' }, mr: 1 }} />
          <Typography
            variant="h5"
            noWrap
            component="a"
            href="/"
            sx={{
              mr: 2,
              display: { xs: 'flex', md: 'none' },
              flexGrow: 1,
              fontFamily: 'monospace',
              fontWeight: 700,
              letterSpacing: '.3rem',
              color: 'inherit',
              textDecoration: 'none',
            }}
          >
            LOGO
          </Typography>
          <Box sx={{ flexGrow: 1, display: { xs: 'none', md: 'flex' } }}>
            {pages
            .filter(page => (page.role && page.role === user?.role || !page.role))
            .map((page) => (
              <Button
                key={page.text}
                onClick={() => { handleCloseNavMenu(); if (page.onClick) page.onClick(); }}
                sx={{ my: 2, color: page.role? 'lightsalmon': 'white', display: 'block' }}
              >
                {page.text}
              </Button>
            ))}
          </Box>

          {!user ? (<>
          <Button
            sx={{ my: 2, color: 'white', display: 'block' }}
            onClick={handleLoginClicked}>
              Log in
          </Button>
          <Button
            sx={{ my: 2, color: 'white', display: 'block' }}
            onClick={handleRegisterClicked}>
              Sign up
          </Button></>) : (
          <Box sx={{ flexGrow: 0 }}>
            <Tooltip title="Open settings">
              <Button sx={{p: '10px', color: 'white', display: 'block', my: 2 }} 
              onClick={handleOpenUserMenu}>
                Hello, {user.name}!
              </Button>
            </Tooltip>
            <Menu
              sx={{ mt: '45px' }}
              id="menu-appbar"
              anchorEl={anchorElUser}
              anchorOrigin={{
                vertical: 'top',
                horizontal: 'right',
              }}
              keepMounted
              transformOrigin={{
                vertical: 'top',
                horizontal: 'right',
              }}
              open={Boolean(anchorElUser)}
              onClose={handleCloseUserMenu}
            >
              {settings.map((setting) => (
                <MenuItem key={setting.text} onClick={() => {if (setting.onClick) setting.onClick()} }>
                  <Typography textAlign="center">{setting.text}</Typography>
                </MenuItem>
              ))}
            </Menu>
          </Box>)}
        </Toolbar>
      </Container>
    </AppBar>
  );
}



// <IconButton onClick={handleOpenUserMenu} sx={{ p: 0 }}>
//  <Avatar alt="Remy Sharp" src="/static/images/avatar/2.jpg" />
// </IconButton>