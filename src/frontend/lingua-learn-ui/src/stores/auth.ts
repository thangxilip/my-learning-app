import { computed, ref } from 'vue'
import { defineStore } from 'pinia'

import type { LoginResponse } from '@/api/auth.generated'

const STORAGE_KEY = 'lingua-learn-auth'

type PersistedAuth = {
  accessToken: string
  refreshToken: string | undefined
  accessTokenExpiresAtUtc: string | undefined
}

function readPersisted(): PersistedAuth | null {
  const raw = sessionStorage.getItem(STORAGE_KEY) ?? localStorage.getItem(STORAGE_KEY)
  if (!raw) return null
  try {
    return JSON.parse(raw) as PersistedAuth
  } catch {
    return null
  }
}

export const useAuthStore = defineStore('auth', () => {
  const accessToken = ref<string | null>(null)
  const refreshToken = ref<string | null>(null)
  const accessTokenExpiresAtUtc = ref<Date | null>(null)

  const isAuthenticated = computed(() => Boolean(accessToken.value))

  function hydrateFromStorage() {
    const data = readPersisted()
    if (!data?.accessToken) return
    accessToken.value = data.accessToken
    refreshToken.value = data.refreshToken ?? null
    accessTokenExpiresAtUtc.value = data.accessTokenExpiresAtUtc
      ? new Date(data.accessTokenExpiresAtUtc)
      : null
  }

  function setSession(response: LoginResponse, rememberMe: boolean) {
    accessToken.value = response.accessToken ?? null
    refreshToken.value = response.refreshToken ?? null
    accessTokenExpiresAtUtc.value = response.accessTokenExpiresAtUtc ?? null

    sessionStorage.removeItem(STORAGE_KEY)
    localStorage.removeItem(STORAGE_KEY)

    const payload: PersistedAuth = {
      accessToken: response.accessToken ?? '',
      refreshToken: response.refreshToken,
      accessTokenExpiresAtUtc: response.accessTokenExpiresAtUtc?.toISOString(),
    }

    const storage = rememberMe ? localStorage : sessionStorage
    storage.setItem(STORAGE_KEY, JSON.stringify(payload))
  }

  function clearSession() {
    accessToken.value = null
    refreshToken.value = null
    accessTokenExpiresAtUtc.value = null
    sessionStorage.removeItem(STORAGE_KEY)
    localStorage.removeItem(STORAGE_KEY)
  }

  return {
    accessToken,
    refreshToken,
    accessTokenExpiresAtUtc,
    isAuthenticated,
    hydrateFromStorage,
    setSession,
    clearSession,
  }
})
