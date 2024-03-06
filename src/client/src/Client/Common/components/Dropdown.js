/* eslint-disable react/forbid-prop-types */
import React from 'react';
import { PropTypes } from 'prop-types';
import {
  MenuItem,
  Select,
  InputLabel,
  FormControl,
} from '@mui/material';

export function Dropdown({
  data,
  textId,
  keyId,
  onChange,
  size,
  value,
  ...rest
}) {
  const mapSimpleType = () => (
    data.map((simpleType) => <MenuItem key={simpleType} value={simpleType}>{simpleType}</MenuItem>)
  );

  const mapComplexType = () => (
    // eslint-disable-next-line max-len
    data.map((complexType) => <MenuItem key={complexType[keyId]} value={complexType[keyId]}>{complexType[textId]}</MenuItem>)
  );

  const mapDataToItem = () => {
    // if label and key id aren't null then assume complex types
    // are used in the data array
    if (textId && keyId) {
      return mapComplexType();
    }

    return mapSimpleType();
  };

  const valueToSimpleType = () => {
    if (textId && keyId) {
      if (data.length) {
        return value[keyId] ?? '';
      }

      return '';
    }

    return value ?? '';
  };

  const FireOnChange = (e) => {
    if (textId && keyId) {
      const item = data.find((i) => i[keyId] === e.target.value);
      onChange(item);
    } else {
      onChange(e.target.value);
    }
  };

  return (
    <FormControl size={size}>
      <InputLabel id={rest.labelId}>{rest.label}</InputLabel>
      <Select
        onChange={FireOnChange}
        value={valueToSimpleType()}
        {...rest}
      >
        {mapDataToItem()}
      </Select>
    </FormControl>
  );
}

Dropdown.propTypes = {
  data: PropTypes.arrayOf(PropTypes.any).isRequired,
  size: PropTypes.string,
  textId: PropTypes.string,
  keyId: PropTypes.string,
  onChange: PropTypes.func.isRequired,
  value: PropTypes.any,
};

Dropdown.defaultProps = {
  textId: '',
  keyId: '',
  size: 'small',
  value: '',
};
