import React, { Component, useEffect } from 'react';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import { Layout } from './components/Layout';
import './custom.css';
import { useDispatch } from 'react-redux';
import { tryLogin } from './features/userSlice';

export default function App() {
  const displayName = App.name;
  const dispatch = useDispatch();

  useEffect(() => {
    dispatch(tryLogin());
  });

    return (
      <Layout>
        <Routes>
          {AppRoutes.map((route, index) => {
            const { element, ...rest } = route;
            return <Route key={index} {...rest} element={element} />;
          })}
        </Routes>
      </Layout>
    );
}
