export type CardStatus = 'new' | 'learning' | 'review' | 'mastered'

export interface FlashCardRow {
  id: string
  front: string
  status: CardStatus
}

export interface FlashcardDeck {
  id: string
  title: string
  description: string
  /** Tailwind background class for the top accent bar and header dot */
  accentClass: string
  cards: FlashCardRow[]
}

export interface FlashcardSummary {
  totalCards: number
  mastered: number
  review: number
  learning: number
  new: number
}

export type FlashcardFilter = 'all' | CardStatus
