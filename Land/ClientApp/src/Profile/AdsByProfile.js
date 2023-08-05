import { Container, Box, Stack, Typography, Button } from '@mui/material';
import React, { useEffect, useState } from 'react';
import axios from "axios";
import { createTheme, ThemeProvider } from "@mui/material/styles";
import NoPhotographyIcon from '@mui/icons-material/NoPhotography';
import VisibilityOffIcon from '@mui/icons-material/VisibilityOff';
import VisibilityIcon from '@mui/icons-material/Visibility';
import EditIcon from '@mui/icons-material/Edit';
import AddOrUpdateAd from '../Add/AddOrUpdateAd';
export default function AdsByProfile({ email }) {
  const [ad, setAd] = useState(undefined);

  const [editAd, setEditAd] = useState(undefined);

  const theme = createTheme({
    breakpoints: {
      values: {
        mobile: 0,
        bigMobile: 740,
        tablet: 1100,
        desktop: 1390,
      },
    },
  });
  useEffect(() => {
    const fetchData = async () => {

      const data = await axios.get(process.env.REACT_APP_SERVER_URL + `ad/getbyuseremail?email=${email}`);
      setAd(data.data);
    }
    fetchData();
  }, [])

  const changeHidden = async (id) => {
    await axios.delete(process.env.REACT_APP_SERVER_URL + `ad?id=${id}`);
    // TODO optimize the next call. do we really need this?
    const data = await axios.get(process.env.REACT_APP_SERVER_URL + `ad/getbyuseremail?email=${email}`);
    setAd(data.data);
  }

  const changeEdit = async (id) => {
    const data = await axios.get(process.env.REACT_APP_SERVER_URL + `ad/getbyid?id=${id}`);
    data.data.price = data.data.price.toString();
    setEditAd(data.data);
  }
  // TODO SHOW POPUP wHEN AD IS activated/deactivated
  // TODO add ad status Оголошення не активне (активне)
  // TODO if ad was edited show message that ad was edited but not created
  return (
    <div>
      {ad &&
        <ThemeProvider theme={theme}>
          <Typography variant='subtitle2' gutterBottom sx={{ maxHeight: '3.4rem', overflow: 'hidden', lineHeight: '1.3' }}>
            Ваші оголошення:
          </Typography>
          <Box
            sx={{
              justifyContent: "center",
              display: "grid",
              gap: "1rem",
              gridTemplateColumns: {
                mobile: "repeat(1, 19rem)",
                bigMobile: "repeat(2, 19rem)",
                tablet: "repeat(3, 19rem)",
                desktop: "repeat(4, 19rem)",
              }
            }}
          >
            {ad.map((x) => (
              <Box key={x.id} sx={{ textAlign: 'initial', border: "1px dotted #5196D9", p: 1 }}>

                <Box sx={{ textAlign: 'center' }}>
                  {x.file1 ? <Box
                    sx={{ height: '6rem' }}
                    component="img"
                    src={"data:image/png;base64," + x.file1} >
                  </Box>
                    :
                    <Box sx={{ height: '6rem', display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
                      <NoPhotographyIcon color='action' sx={{ fontSize: 90 }} />
                    </Box>}
                </Box>
                <Stack sx={{ height: '2.5rem' }}>
                  <Typography variant='subtitle2' gutterBottom sx={{ maxHeight: '3.4rem', overflow: 'hidden', lineHeight: '1.3' }}>
                    {x.name}
                  </Typography>
                </Stack>
                <Stack direction={'row'} justifyContent="center" spacing={2} sx={{ mb: '2px' }}>
                  <Button size='small' variant="contained" startIcon={!x.hidden ? <VisibilityOffIcon /> : <VisibilityIcon />} onClick={async () => { await changeHidden(x.id); }} >{!x.hidden ? 'Деактивувати' : 'Активувати'}</Button>
                  <Button size='small' variant="contained" startIcon={<EditIcon />} onClick={async () => { await changeEdit(x.id); }}>Змінити</Button>
                </Stack>
              </Box>
            ))}
          </Box>
        </ThemeProvider>
      }
      {editAd !== undefined && <AddOrUpdateAd defaultValue={editAd} />}
    </div>
  )
}