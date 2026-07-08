import React, { useState, useEffect } from 'react';
import StatusBadge from '../Badges/StatusBadge';
import LoadingSpinner from '../Common/LoadingSpinner';
import { useAuth } from '../../Context/AuthContext';
import {
  getAllAppointmentsOfThePatient,
  getAllAppointmentsOfTheDoctor,
  getAllAppointments
} from '../../api/appointment';
import ConsultationReportModel from '../Models/ConsultationReportModel';
import DiagnosisModel from '../Models/DiagnosisModel';
import PrescriptionModel from '../Models/PrescriptionModel';
import RescheduleModel from '../Models/RescheduleModel';
import BookAppointmentModel from '../Models/BookAppointmentModel';
import StatusModel from '../Models/StatusModel';

function AppointmentTable({
  showPatientColumn = true,
  showDoctorColumn = false,
  role,
  historyOnly = false
}) {
  const { profileId } = useAuth();
  const [appointments, setAppointments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const [selectedApp, setSelectedApp] = useState(null);
  const [modelType, setModelType] = useState(null);
  const [targetStatus, setTargetStatus] = useState(null);
  const [rescheduleItem, setRescheduleItem] = useState(null);

  const maroonColor = '#8b1031';

  useEffect(() => {
    if ((role === 'patient' || role === 'doctor') && !profileId) {
      return;
    }
    loadAppointmentsData();
  }, [role, profileId, historyOnly]);

  const loadAppointmentsData = async () => {
    setLoading(true);
    setError(null);
    try {
      let data = [];
      if (role === 'patient') {
        data = await getAllAppointmentsOfThePatient(profileId);
        if (Array.isArray(data) && historyOnly) {
          data = data.filter(app => app.recordId && app.recordId !== 0);
        }
      } else if (role === 'doctor') {
        data = await getAllAppointmentsOfTheDoctor(profileId);
      } else if (role === 'admin') {
        data = await getAllAppointments();
      }

      if (Array.isArray(data)) {
        setAppointments(data);
      } else {
        setAppointments([]);
      }
    } catch (err) {
      console.error(err);
      setError('Could not retrieve appointments records.');
    } finally {
      setLoading(false);
    }
  };

  const handleRescheduleSuccess = (appointmentId, newDate, newTime) => {
    setAppointments(prev => prev.map(app =>
      app.appointmentId === appointmentId
        ? { ...app, appointmentDate: newDate, preferedTime: newTime + ":00", status: "Rescheduled" }
        : app
    ));
  };

  const openRescheduleModel = (app) => {
    setRescheduleItem(app);
  };

  const openRecordModel = (app) => {
    setSelectedApp(app);
    setModelType('record');
  };

  const openPrescriptionModel = (app) => {
    setSelectedApp(app);
    setModelType('prescription');
  };

  const openViewReportModel = (app) => {
    setSelectedApp(app);
    setModelType('viewReport');
  };

  return (
    <div className="card border-0 shadow-sm rounded-4 overflow-hidden">
      <div className="card-header bg-white border-bottom d-flex justify-content-between align-items-center py-3 px-4">
        <h5 className="fw-bold mb-0 text-dark">Appointment Records</h5>
        <div className="d-flex gap-2">
          {role === 'admin' && (
            <button
              onClick={() => {
                setModelType('book');
              }}
              className="btn btn-sm text-white fw-bold shadow-sm d-inline-flex align-items-center"
              style={{ backgroundColor: maroonColor }}
            >
              <i className="bi bi-plus-lg me-1"></i> Book Appointment
            </button>
          )}
          <button
            onClick={loadAppointmentsData}
            disabled={loading}
            className="btn btn-sm btn-outline-secondary fw-bold shadow-sm d-inline-flex align-items-center"
          >
            {loading ? (
              <span className="spinner-border spinner-border-sm me-1" role="status" aria-hidden="true"></span>
            ) : (
              <i className="bi bi-arrow-clockwise me-1"></i>
            )}
            {loading ? 'Refreshing...' : 'Refresh'}
          </button>
        </div>
      </div>

      <div className="card-body p-0">
        {loading && appointments.length === 0 ? (
          <div className="d-flex justify-content-center py-5">
            <LoadingSpinner message="Loading appointments..." />
          </div>
        ) : error ? (
          <div className="p-4">
            <div className="alert alert-danger d-flex align-items-center shadow-sm mb-0" role="alert">
              <i className="bi bi-exclamation-triangle-fill me-2"></i>
              <div>{error}</div>
            </div>
          </div>
        ) : appointments.length === 0 ? (
          <div className="text-center py-5">
            <i className="bi bi-calendar-x text-muted mb-3 d-block" style={{ fontSize: '4rem' }}></i>
            <h4 className="fw-bold text-dark">No Appointments Found</h4>
            <p className="text-muted mb-0">No records exist for the selected criteria.</p>
          </div>
        ) : (
          <div className="table-responsive">
            <table className="table table-hover align-middle mb-0">
              <thead className="table-light text-secondary">
                <tr>
                  {showPatientColumn && <th scope="col" className="py-3 px-4 fw-bold border-bottom-0 text-start">Patient Name</th>}
                  {showDoctorColumn && <th scope="col" className="py-3 px-4 fw-bold border-bottom-0 text-start">Doctor Name</th>}
                  <th scope="col" className="py-3 px-4 fw-bold border-bottom-0 text-start">Date</th>
                  <th scope="col" className="py-3 px-4 fw-bold border-bottom-0 text-start">Preferred Time</th>
                  <th scope="col" className="py-3 px-4 fw-bold border-bottom-0 text-start">Nature of Visit</th>
                  <th scope="col" className="py-3 px-4 fw-bold border-bottom-0 text-start">Status</th>
                  <th scope="col" className="py-3 px-4 fw-bold border-bottom-0 text-end">Actions</th>
                </tr>
              </thead>
              <tbody className="border-top-0">
                {appointments.map((app) => {
                  const timeVal = app.preferedTime || app.preferredTime || app.PreferedTime || app.PreferredTime || app.appointmentTime || app.AppointmentTime || '';
                  const visitVal = app.natureOfVisit || app.NatureOfVisit || '';

                  return (
                    <tr key={app.appointmentId}>
                      {showPatientColumn && (
                        <td className="py-3 px-4 fw-bold text-dark text-start">
                          {app.patientName}
                        </td>
                      )}
                      {showDoctorColumn && (
                        <td className="py-3 px-4 fw-bold text-dark text-start">
                          {app.doctorName || 'Not Assigned'}
                        </td>
                      )}
                      <td className="py-3 px-4 text-muted text-start">
                        {app.appointmentDate?.split('T')[0] || app.appointmentDate}
                      </td>
                      <td className="py-3 px-4 text-muted text-start">
                        {timeVal?.slice(0, 5) || timeVal}
                      </td>
                      <td className="py-3 px-4 text-muted text-start">
                        {visitVal}
                      </td>
                      <td className="py-3 px-4 text-start">
                        <StatusBadge status={app.status} />
                      </td>
                      <td className="py-3 px-4 text-end">
                        <div className="d-inline-flex gap-2 justify-content-end">
                          {role === 'patient' && (
                            <>
                              {app.status !== 'Completed' && app.status !== 'Cancelled' && (
                                <>
                                  <button
                                    onClick={() => openRescheduleModel(app)}
                                    className="btn btn-sm btn-outline-secondary fw-bold shadow-sm"
                                  >
                                    Reschedule
                                  </button>
                                  <button
                                    onClick={() => {
                                      setSelectedApp(app);
                                      setTargetStatus('Cancelled');
                                      setModelType('status');
                                    }}
                                    className="btn btn-sm btn-outline-danger fw-bold shadow-sm"
                                  >
                                    Cancel
                                  </button>
                                </>
                              )}
                              {app.status === 'Completed' && app.recordId && app.recordId !== 0 && (
                                <button
                                  onClick={() => openViewReportModel(app)}
                                  className="btn btn-sm btn-outline-secondary fw-bold shadow-sm d-inline-flex align-items-center"
                                >
                                  <i className="bi bi-file-earmark-text me-2"></i> View Report
                                </button>
                              )}
                            </>
                          )}

                          {role === 'doctor' && (
                            <>
                              {(app.status === 'Requested' || app.status === 'Rescheduled') && (
                                <div className="d-flex gap-2">
                                  <button
                                    onClick={() => {
                                      setSelectedApp(app);
                                      setTargetStatus('Upcoming');
                                      setModelType('status');
                                    }}
                                    className="btn btn-sm btn-success fw-bold shadow-sm"
                                  >
                                    Accept
                                  </button>
                                  <button
                                    onClick={() => {
                                      setSelectedApp(app);
                                      setTargetStatus('Cancelled');
                                      setModelType('status');
                                    }}
                                    className="btn btn-sm btn-outline-danger fw-bold shadow-sm"
                                  >
                                    Decline
                                  </button>
                                  <button
                                    onClick={() => openRescheduleModel(app)}
                                    className="btn btn-sm btn-outline-secondary fw-bold shadow-sm"
                                  >
                                    Reschedule
                                  </button>
                                </div>
                              )}

                              {(app.status === 'Scheduled' || app.status === 'Upcoming') && (
                                <div className="d-flex gap-2">
                                  <button
                                    onClick={() => openRecordModel(app)}
                                    className="btn btn-sm btn-primary fw-bold shadow-sm"
                                  >
                                    Diagnose & Complete
                                  </button>
                                  <button
                                    onClick={() => openRescheduleModel(app)}
                                    className="btn btn-sm btn-outline-secondary fw-bold shadow-sm"
                                  >
                                    Reschedule
                                  </button>
                                </div>
                              )}

                              {app.status === 'Completed' && (
                                <div className="d-flex gap-2">
                                  <button
                                    onClick={() => openViewReportModel(app)}
                                    className="btn btn-sm btn-outline-secondary fw-bold shadow-sm"
                                  >
                                    View Report
                                  </button>
                                  <button
                                    onClick={() => openRecordModel(app)}
                                    className="btn btn-sm btn-outline-primary fw-bold shadow-sm"
                                  >
                                    Edit Diagnosis
                                  </button>
                                  <button
                                    onClick={() => openPrescriptionModel(app)}
                                    className="btn btn-sm btn-success fw-bold shadow-sm"
                                  >
                                    Prescribe Meds
                                  </button>
                                </div>
                              )}
                            </>
                          )}

                          {role === 'admin' && (
                            <>
                              {(app.status === 'Requested'|| app.status === 'Rescheduled') && (
                                <div className="d-flex gap-2 justify-content-end">
                                  <button
                                    onClick={() => {
                                      setSelectedApp(app);
                                      setTargetStatus('Upcoming');
                                      setModelType('status');
                                    }}
                                    className="btn btn-sm btn-success fw-bold shadow-sm"
                                  >
                                    Accept
                                  </button>
                                  <button
                                    onClick={() => {
                                      setSelectedApp(app);
                                      setTargetStatus('Cancelled');
                                      setModelType('status');
                                    }}
                                    className="btn btn-sm btn-outline-danger fw-bold shadow-sm"
                                  >
                                    Decline
                                  </button>
                                  <button onClick={() => openRescheduleModel(app)} className="btn btn-sm btn-outline-secondary fw-bold shadow-sm">
                                    Reschedule
                                  </button>
                                </div>
                              )}

                              {app.status === 'Completed' && (
                                <div className="d-flex gap-2">
                                  <button
                                    onClick={() => openViewReportModel(app)}
                                    className="btn btn-sm btn-outline-secondary fw-bold shadow-sm"
                                  >
                                    View Report
                                  </button>
                                </div>
                              )}


                              {(app.status === 'Scheduled' || app.status === 'Upcoming') && (
                                <div className="d-flex gap-2 justify-content-end">
                                  <button onClick={() => openRescheduleModel(app)} className="btn btn-sm btn-outline-secondary fw-bold shadow-sm">
                                    Reschedule
                                  </button>
                                  <button
                                    onClick={() => {
                                      setSelectedApp(app);
                                      setTargetStatus('Cancelled');
                                      setModelType('status');
                                    }}
                                    className="btn btn-sm btn-outline-danger fw-bold shadow-sm"
                                  >
                                    Cancel
                                  </button>
                                </div>
                              )}
                            </>
                          )}
                        </div>
                      </td>
                    </tr>
                  );
                })}
              </tbody>
            </table>
          </div>
        )}
      </div>

      {modelType === 'viewReport' && selectedApp && (
        <ConsultationReportModel
          recordId={selectedApp.recordId}
          onClose={() => { setModelType(null); setSelectedApp(null); }}
        />
      )}

      {modelType === 'record' && selectedApp && (
        <DiagnosisModel
          onClose={() => { setModelType(null); setSelectedApp(null); }}
          appointment={selectedApp}
          onSuccess={() => loadAppointmentsData()}
        />
      )}

      {modelType === 'prescription' && selectedApp && (
        <PrescriptionModel
          onClose={() => { setModelType(null); setSelectedApp(null); }}
          appointment={selectedApp}
          onSuccess={() => loadAppointmentsData()}
        />
      )}

      {rescheduleItem && (
        <RescheduleModel
          onClose={() => setRescheduleItem(null)}
          appointment={rescheduleItem}
          onSuccess={handleRescheduleSuccess}
        />
      )}

      {role === 'admin' && modelType === 'book' && (
        <BookAppointmentModel
          isModal={true}
          showPatientSelect={true}
          onClose={() => setModelType(null)}
          onSuccess={() => {
            setModelType(null);
            loadAppointmentsData();
          }}
        />
      )}

      {modelType === 'status' && selectedApp && (
        <StatusModel
          appointment={selectedApp}
          newStatus={targetStatus}
          onClose={() => {
            setModelType(null);
            setSelectedApp(null);
            setTargetStatus(null);
          }}
          onSuccess={() => {
            setModelType(null);
            setSelectedApp(null);
            setTargetStatus(null);
            loadAppointmentsData();
          }}
        />
      )}
    </div>
  );
}

export default AppointmentTable;
