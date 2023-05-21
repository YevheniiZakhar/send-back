import React from 'react';
import { Route, Routes } from 'react-router-dom';
import { Main } from '../Pages/Main';
import { Layout } from './Layout';

const Navigation = () => {
  return (
    <>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route index path="about" element={<Main />} />
          <Route path="contact" element={<>contact</>} />
          <Route path="*" />
        </Route>
      </Routes>
    </>
  );
};

export default Navigation;
