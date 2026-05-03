import { computed, ref } from 'vue'

import type {
  FlashcardDeck,
  FlashcardFilter,
  FlashcardSummary,
} from '@/features/flashcards/types'

function initialDecks(): FlashcardDeck[] {
  return [
    {
      id: 'deck-es',
      title: 'Spanish basics',
      description: 'Common greetings and phrases',
      accentClass: 'bg-rose-400',
      cards: [
        { id: 'c1', front: 'Hola', status: 'learning' },
        { id: 'c2', front: 'Gracias', status: 'mastered' },
        { id: 'c3', front: 'Por favor', status: 'new' },
      ],
    },
    {
      id: 'deck-capitals',
      title: 'Capitals',
      description: 'World capitals drill',
      accentClass: 'bg-cyan-400',
      cards: [
        { id: 'c4', front: 'France', status: 'mastered' },
        { id: 'c5', front: 'Japan', status: 'learning' },
      ],
    },
    {
      id: 'deck-w1',
      title: 'Week 1 vocab',
      description: 'Course pack — week one',
      accentClass: 'bg-violet-500',
      cards: [],
    },
  ]
}

function deckStats(deck: FlashcardDeck) {
  const total = deck.cards.length
  const mastered = deck.cards.filter((c) => c.status === 'mastered').length
  const pct = total === 0 ? 0 : Math.round((mastered / total) * 100)
  return { total, mastered, pct }
}

export function useFlashcards() {
  const decks = ref<FlashcardDeck[]>(initialDecks())
  const statusFilter = ref<FlashcardFilter>('all')
  const draftCardText = ref<Record<string, string>>({})

  const summary = computed<FlashcardSummary>(() => {
    const all = decks.value.flatMap((d) => d.cards)
    const totalCards = all.length
    const mastered = all.filter((c) => c.status === 'mastered').length
    const review = all.filter((c) => c.status === 'review').length
    const learning = all.filter((c) => c.status === 'learning').length
    const newCount = all.filter((c) => c.status === 'new').length
    return { totalCards, mastered, review, learning, new: newCount }
  })

  const summaryLine = computed(() => {
    const s = summary.value
    if (s.totalCards === 0) {
      return 'No cards yet'
    }
    return `${s.mastered}/${s.totalCards} mastered · ${s.review} review · ${s.learning} learning · ${s.new} new`
  })

  const filteredDecks = computed(() => {
    if (statusFilter.value === 'all') {
      return decks.value
    }
    const f = statusFilter.value
    return decks.value.filter((deck) => deck.cards.some((c) => c.status === f))
  })

  function setStatusFilter(filter: FlashcardFilter) {
    statusFilter.value = filter
  }

  function setDraftForDeck(deckId: string, value: string) {
    draftCardText.value = { ...draftCardText.value, [deckId]: value }
  }

  function addCardToDeck(deckId: string) {
    const text = (draftCardText.value[deckId] ?? '').trim()
    if (!text) return

    const deck = decks.value.find((d) => d.id === deckId)
    if (!deck) return

    const id = `c-${crypto.randomUUID()}`
    deck.cards.push({ id, front: text, status: 'new' })
    draftCardText.value = { ...draftCardText.value, [deckId]: '' }
  }

  function addDeck() {
    const id = `deck-${crypto.randomUUID()}`
    decks.value.push({
      id,
      title: 'New deck',
      description: 'Rename and add cards to get started',
      accentClass: 'bg-zinc-400',
      cards: [],
    })
  }

  return {
    decks,
    statusFilter,
    summary,
    summaryLine,
    filteredDecks,
    draftCardText,
    setStatusFilter,
    setDraftForDeck,
    addCardToDeck,
    addDeck,
    deckStats,
  }
}
