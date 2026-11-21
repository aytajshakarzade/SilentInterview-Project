import { Navigate, useLocation } from 'react-router-dom';
import { useAuth } from '../../hooks/useAuth';
import { ROUTES } from '../../constants/routes';
import AuthLoadingScreen from './AuthLoadingScreen';

/**
 * Guards routes behind authentication + optional role check.
 * roles: ['RECRUITER'] — uppercase strings matching user.role
 */
export default function ProtectedRoute({ children, roles }) {
  const { user, isAuthenticated, loading } = useAuth();
  const location = useLocation();

  // Do not mount protected pages until refresh-cookie hydration has completed.
  // In particular, BillingPage must not call /subscription/sync with no
  // in-memory access token while /Auth/refresh is already running.
  if (loading) return <AuthLoadingScreen />;

  // Not logged in → send to login, remember where they came from
  if (!isAuthenticated) {
    return <Navigate to={ROUTES.LOGIN} state={{ from: location }} replace />;
  }

  // Logged in but wrong role → forbidden
  if (roles && roles.length > 0 && !roles.includes(user?.role?.toUpperCase())) {
    return <Navigate to={ROUTES.FORBIDDEN} replace />;
  }

  return children;
}
