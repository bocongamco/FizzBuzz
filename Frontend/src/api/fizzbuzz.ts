import { api } from './client';
import * as T from './types';

export async function createGame(body: T.CreateGameRequest) {
  return (await api.post<T.GameResponse>('/Games', body)).data;
}
export async function getGame(id: string) {
  return (await api.get<T.GameResponse>(`/Games/${id}`)).data;
}
export async function startSession(body: T.StartSessionRequest) {
  return (await api.post<T.StartSessionResponse>('/Session/start', body)).data;
}
export async function submitAnswer(sessionId: string, body: T.SubmitAnswerRequest) {
  return (await api.post<T.SubmitAnswerResponse>(`/Session/${sessionId}/submit`, body)).data;
}
export async function nextNumber(sessionId: string) {
  const res = await api.get<T.NextNumberResponse>(`/Session/${sessionId}/next`, { validateStatus: () => true });
  if (res.status === 200) return res.data;
  if (res.status === 204) return null; // exhausted
  if (res.status === 410) throw new Error('expired');
  throw new Error(`Unexpected ${res.status}`);
}

export async function listGames() {
  return (await api.get<T.GameListItem[]>('/Games')).data;
}

export async function deleteGame(id: string) {
  await api.delete(`/Games/${id}`);
}
export async function summary(sessionId: string) {
  return (await api.get<T.SummaryResponse>(`/Session/${sessionId}/summary`)).data;
}