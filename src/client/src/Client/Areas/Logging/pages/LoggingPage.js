/* eslint-disable object-curly-newline */
import React, { useEffect } from 'react';
import { Box, Grid, Button } from '@mui/material';
import { DataGrid } from '@mui/x-data-grid';
import { useMonitoringApi } from '../../../Api/RestApi';

const columns = [
  {
    field: 'id',
    headerName: 'Id',
    type: 'number',
    width: 60,
  },
  {
    field: 'addedDate',
    headerName: 'Added Date',
    width: 200,
  },
  {
    field: 'level',
    headerName: 'Log Level',
    width: 100,
  },
  {
    field: 'message',
    headerName: 'Message',
    width: 300,
  },
  {
    field: 'exception',
    headerName: 'Exception',
    width: 200,
  },
];

export function LoggingPage() {
  const {
    getAllLogs,
    deleteAllLogs,
  } = useMonitoringApi();

  useEffect(() => {
    getAllLogs.request();
  }, []);

  return (
    <Grid
      container
      sx={{
        padding: 2,
      }}
      rowSpacing={1}
    >
      <Grid item xs={12}>
        <Button
          onClick={getAllLogs.request}
          variant="contained"
          color="secondary"
          size="small"
          sx={{
            marginRight: 1,
          }}
        >
          Refresh
        </Button>
        <Button
          onClick={() => {
            deleteAllLogs.request();
            getAllLogs.resetData();
          }}
          variant="contained"
          color="warning"
          size="small"
          sx={{
            marginRight: 1,
          }}
        >
          Delete
        </Button>
      </Grid>
      <Grid
        item
        xs={12}
      >
        <Box sx={{ height: 325, width: '100%' }}>
          <DataGrid
            rows={getAllLogs.data}
            columns={columns}
            initialState={{
              pagination: {
                paginationModel: {
                  pageSize: 10,
                },
              },
            }}
            pageSizeOptions={[10, 50, 100]}
          />
        </Box>
      </Grid>
    </Grid>
  );
}
