export type CreateRuleDto = { divisor: 
  number; word: 
  string; order?:
  number };
export type CreateGameRequest = {
  name: string; 
  author: string; 
  min: number; 
  max: number; 
  rules: CreateRuleDto[];
};
export type RuleDto = { divisor: 
  number; 
  word: string; 
  order: number };
export type GameResponse = {
  id: string; 
  name: string; 
  author: string; 
  min: number; max: number;
  rules: RuleDto[];
};

export type StartSessionRequest = { 
  gameId: string; 
  durationSeconds: number };
export type StartSessionResponse = { 
  sessionId: string; 
  endsAt: string; 
  firstNumber: number };

export type SubmitAnswerRequest = { number: number; answer: string };
export type SubmitAnswerResponse = {
  expected: string; 
  isCorrect: boolean; 
  scoreCorrect: number; 
  scoreIncorrect: number;
  number?: number; submitted?: string;
};

export type NextNumberResponse = { 
  number: number };
export type SummaryResponse = {
   scoreCorrect: number; 
   scoreIncorrect: number; 
   total: number };

export type GameListItem = {
  id: string;
  name: string;
  author: string;
  createdAt: string;
  rules: number;
  sessions: number;
  bestScore: number;
};