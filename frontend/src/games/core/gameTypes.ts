export const GameIds = {
  IMPOSTERS: "imposters"
} as const;

export type GameId = typeof GameIds[keyof typeof GameIds];