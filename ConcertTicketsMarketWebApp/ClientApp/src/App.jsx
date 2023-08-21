import { Route, Routes } from 'react-router-dom'
import AppRoutes from './AppRoutes'
import './App.css'
import Layout from './components/Layout.jsx'
import { useEffect } from 'react'

function App() { 
  return (<>
    <Layout>
      <Routes>
        {AppRoutes.map((route, index) => {
          const { element, ...rest } = route;
          return <Route key={index} {...rest} element={element} />;
        })}
      </Routes>
    </Layout>
  </>)
}

export default App