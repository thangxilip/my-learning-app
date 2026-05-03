import type { CardStatus } from './types'

/** Badge + dot colors aligned with the reference UI */
export const statusPresentation: Record<
  CardStatus,
  { dot: string; badge: string }
> = {
  new: {
    dot: 'bg-zinc-400',
    badge:
      'border-transparent bg-zinc-100 text-zinc-700 dark:bg-zinc-800 dark:text-zinc-200',
  },
  learning: {
    dot: 'bg-rose-500',
    badge:
      'border-transparent bg-rose-100 text-rose-900 dark:bg-rose-950 dark:text-rose-100',
  },
  review: {
    dot: 'bg-teal-500',
    badge:
      'border-transparent bg-teal-100 text-teal-900 dark:bg-teal-950 dark:text-teal-100',
  },
  mastered: {
    dot: 'bg-emerald-500',
    badge:
      'border-transparent bg-emerald-100 text-emerald-900 dark:bg-emerald-950 dark:text-emerald-100',
  },
}

export const statusLabels: Record<CardStatus, string> = {
  new: 'New',
  learning: 'Learning',
  review: 'Review',
  mastered: 'Mastered',
}
