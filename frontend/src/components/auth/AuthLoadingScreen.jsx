import { Box, CircularProgress } from '@mui/material';

/** Shared loading state while the HttpOnly refresh-cookie session is restored. */
export default function AuthLoadingScreen() {
  return (
    <Box
      role="status"
      aria-label="Restoring your session"
      sx={{ minHeight: '100vh', display: 'grid', placeItems: 'center' }}
    >
      <CircularProgress size={30} />
    </Box>
  );
}
