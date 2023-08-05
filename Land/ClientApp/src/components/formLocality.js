import React, { useState } from 'react';
import Autocomplete from '@mui/material/Autocomplete';
import axios from "axios";
import TextField from '@mui/material/TextField';
import { Controller, useFormContext } from 'react-hook-form';
import { FormControl, FormHelperText } from '@mui/material';

export default function FormLocality({edit}) {
  const [lOptions, setLOptions] = useState([]);

  const {
    control,
    formState: { errors },
  } = useFormContext();

  const onAutocompleteChange = async (event, value) => {
    if (value !== 'undefined') {
        if (value.length > 1 && event.type !== "click") {
          const res = await axios.get(process.env.REACT_APP_SERVER_URL+`ad/locality?str=${value}`);
          if (res.status === 200) {
            setLOptions(res.data);
          }
        } else {
          setLOptions([]);
        }
      }
    }

  return (
    <>{!edit && <Controller control={control} name={'localityId'} render={({ field })=> (
      <FormControl sx={{marginTop: errors['phone'] ? '1rem' : ''}} fullWidth error={!!errors['localityId']}>
        <Autocomplete
          {...field}
          openOnFocus={true}
          loading={true}
          getOptionLabel={(o) => o.locality}
          loadingText="Введіть назву міста або села українською мовою"
          options={lOptions}
          sx={{ width: '100%' }}
          onInputChange={onAutocompleteChange}
          onChange={field.onChange}
          renderOption={(props, option) => {
            return (
                <li {...props} value={option.id} key={option.id}>
                {option.locality}
                </li>
            );
          }}
          renderInput={(params) => <TextField {...params} defaultValue='aaa' label="Місцезнаходження *" />}
        />
        <FormHelperText>{errors['localityId'] ? errors['localityId'].message : ''}</FormHelperText>
      </FormControl>
    )}/>}</>
    
  )
}