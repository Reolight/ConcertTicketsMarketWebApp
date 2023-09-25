import React, { Component, useEffect } from 'react';
import { Outlet, Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import AuthorizeRoute from './components/api-authorization/AuthorizeRoute';
import { Layout } from './components/Layout';
import './custom.css';
import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { YMaps } from '@pbe/react-yandex-maps';

export default function App(props) {
  
  return (
    <LocalizationProvider dateAdapter={AdapterDayjs} adapterLocale="en">
      <Layout>
        <YMaps 
          query={{
              apikey: '8b70e741-01f8-472d-829a-9de7165b8135', 
              load: "package.full",
              lang: 'en_RU'
          }}
        >
          <Outlet />
        </YMaps>
      </Layout>
    </LocalizationProvider>
  );

}
