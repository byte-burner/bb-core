import React, { useState } from 'react';
import { PropTypes } from 'prop-types';
import {
  Drawer,
  Box,
  List,
  ListItem,
  IconButton,
  Grid,
  Divider,
  Typography,
  ListItemButton,
} from '@mui/material';
import ArrowForwardIosIcon from '@mui/icons-material/ArrowForwardIos';
import DeleteForeverIcon from '@mui/icons-material/DeleteForever';

export function OpenToolbarDrawerButton({
  onItemClick, onTrashClick, items, current,
}) {
  const [open, setOpen] = useState(false);

  const handleItemClick = (terminal) => {
    onItemClick(terminal);
    setOpen(false);
  };

  const handleTrashClick = (e, terminal) => {
    e.stopPropagation();
    onTrashClick(terminal);
    setOpen(false);
  };

  const setLineItemBackground = (item) => (item?.pid === current?.pid ? { backgroundColor: 'rgba(255, 255, 255, 0.3)' } : null);

  return (
    <Box>
      <IconButton size="small" onClick={() => setOpen(true)}>
        <ArrowForwardIosIcon />
      </IconButton>
      <Drawer
        anchor="right"
        PaperProps={{
          sx: { width: '40%' },
        }}
        open={open}
        onClose={() => setOpen(false)}
      >
        <List>
          {items.map((terminal) => (
            <Box key={terminal.pid}>
              <ListItem disablePadding>
                <ListItemButton
                  onClick={() => handleItemClick(terminal)}
                  sx={() => setLineItemBackground(terminal)}
                >
                  <Grid container>
                    <Grid
                      item
                      xs={6}
                      sx={{
                        display: 'flex',
                        justifyContent: 'flex-start',
                        alignItems: 'center',
                      }}
                    >
                      <ArrowForwardIosIcon sx={{ marginRight: 2 }} />
                      <Typography>{`${terminal?.pid} - ${terminal?.type}`}</Typography>
                    </Grid>
                    <Grid
                      item
                      xs={6}
                      sx={{
                        display: 'flex',
                        justifyContent: 'flex-end',
                        alignItems: 'center',
                      }}
                    >
                      <IconButton size="small" onClick={(e) => handleTrashClick(e, terminal)}>
                        <DeleteForeverIcon />
                      </IconButton>
                    </Grid>
                  </Grid>
                </ListItemButton>
              </ListItem>
              <Divider />
            </Box>
          ))}
        </List>
      </Drawer>
    </Box>
  );
}

OpenToolbarDrawerButton.propTypes = {
  onItemClick: PropTypes.func.isRequired,
  onTrashClick: PropTypes.func.isRequired,
  items: PropTypes.arrayOf(PropTypes.any.isRequired).isRequired,
  // eslint-disable-next-line react/forbid-prop-types
  current: PropTypes.any,
};

OpenToolbarDrawerButton.defaultProps = {
  current: null,
};
