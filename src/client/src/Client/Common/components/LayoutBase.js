import React from 'react';
import { PropTypes } from 'prop-types';
import { Box } from '@mui/material';

export function LayoutBase({ header, body }) {
  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',
        height: '100%',
        width: '100%',
      }}
    >

      {header && (
        <Box>
          {header}
        </Box>
      )}

      <Box sx={{ flex: 1 }}>
        {body}
      </Box>

    </Box>
  );
}

LayoutBase.propTypes = {
  header: PropTypes.element,
  body: PropTypes.element.isRequired,
};

LayoutBase.defaultProps = {
  header: null,
};
