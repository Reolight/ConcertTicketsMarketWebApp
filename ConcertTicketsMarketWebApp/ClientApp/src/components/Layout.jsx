import { Container } from '@mui/material';
import NavMenu from './NavMenu';

export default function Layout(props){
  const displayName = Layout.name;

    return (
      <div>
        <NavMenu />
        <Container sx={{ marginTop: 10 }}>
          {props.children}
        </Container>
      </div>
    )
}
